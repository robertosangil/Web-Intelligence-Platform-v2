"use client";

import { FormEvent, useEffect, useState } from "react";
import { getPermissions, getProfile, login, logout, refresh, type PermissionsResponse, type ProfileResponse } from "@/lib/api";

const ACCESS_KEY = "wip.access";
const REFRESH_KEY = "wip.refresh";

export function AccessPanel() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [profile, setProfile] = useState<ProfileResponse | null>(null);
  const [permissions, setPermissions] = useState<PermissionsResponse | null>(null);
  const [status, setStatus] = useState<"idle" | "loading" | "error">("idle");
  const [message, setMessage] = useState("");

  useEffect(() => {
    const token = sessionStorage.getItem(ACCESS_KEY);
    if (!token) return;
    const restore = async () => {
      try {
        const [nextProfile, nextPermissions] = await Promise.all([getProfile(token), getPermissions(token)]);
        setProfile(nextProfile); setPermissions(nextPermissions);
      } catch {
        const refreshToken = sessionStorage.getItem(REFRESH_KEY);
        if (!refreshToken) throw new Error("No refresh session");
        const tokens = await refresh(refreshToken);
        sessionStorage.setItem(ACCESS_KEY, tokens.accessToken);
        sessionStorage.setItem(REFRESH_KEY, tokens.refreshToken);
        const [nextProfile, nextPermissions] = await Promise.all([getProfile(tokens.accessToken), getPermissions(tokens.accessToken)]);
        setProfile(nextProfile); setPermissions(nextPermissions);
      }
    };
    restore().catch(() => { sessionStorage.removeItem(ACCESS_KEY); sessionStorage.removeItem(REFRESH_KEY); });
  }, []);

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault(); setStatus("loading"); setMessage("");
    try {
      const tokens = await login(email.trim(), password);
      sessionStorage.setItem(ACCESS_KEY, tokens.accessToken);
      sessionStorage.setItem(REFRESH_KEY, tokens.refreshToken);
      const [nextProfile, nextPermissions] = await Promise.all([getProfile(tokens.accessToken), getPermissions(tokens.accessToken)]);
      setProfile(nextProfile); setPermissions(nextPermissions); setPassword(""); setStatus("idle");
    } catch { setStatus("error"); setMessage("Sign-in failed. Check your email and password, then try again."); }
  }

  async function signOut() {
    const refreshToken = sessionStorage.getItem(REFRESH_KEY);
    if (refreshToken) await logout(refreshToken).catch(() => undefined);
    sessionStorage.removeItem(ACCESS_KEY); sessionStorage.removeItem(REFRESH_KEY);
    setProfile(null); setPermissions(null); setMessage("");
  }

  if (profile) return <section className="access-panel" aria-live="polite"><div className="panel-heading"><div><p className="eyebrow">Signed in</p><h2>{profile.firstName || profile.lastName ? `${profile.firstName ?? ""} ${profile.lastName ?? ""}`.trim() : profile.email}</h2><p className="muted">{profile.email}</p></div><button className="secondary-button" type="button" onClick={signOut}>Sign out</button></div><p className="access-label">Your access</p><p>{permissions?.roles.length ? permissions.roles.join(", ") : "No roles assigned"}</p><p className="muted">{permissions?.modules.length ?? 0} permitted modules</p></section>;

  return <section className="access-panel"><p className="eyebrow">Account access</p><h2>Sign in to continue</h2><p className="muted">Use your WIP v2 account to view the access granted to you.</p><form onSubmit={submit}><label htmlFor="email">Email</label><input id="email" type="email" autoComplete="email" required value={email} onChange={(event) => setEmail(event.target.value)} /><label htmlFor="password">Password</label><input id="password" type="password" autoComplete="current-password" required value={password} onChange={(event) => setPassword(event.target.value)} />{message && <p className="form-error" role="alert">{message}</p>}<button className="primary-button" type="submit" disabled={status === "loading"}>{status === "loading" ? "Signing in…" : "Sign in"}</button></form></section>;
}
