"use client";

import { useEffect, useState } from "react";
import { apiBaseUrl, getApiHealth, type HealthResponse } from "@/lib/api";

type ApiState =
  | { kind: "checking" }
  | { kind: "available"; health: HealthResponse }
  | { kind: "unavailable" };

export function ApiStatus() {
  const [state, setState] = useState<ApiState>({ kind: "checking" });

  useEffect(() => {
    const controller = new AbortController();

    getApiHealth(controller.signal)
      .then((health) => setState({ kind: "available", health }))
      .catch(() => {
        if (!controller.signal.aborted) {
          setState({ kind: "unavailable" });
        }
      });

    return () => controller.abort();
  }, []);

  return (
    <section className="status" aria-live="polite">
      <h2>Foundation status</h2>
      <p>
        API: <code>{apiBaseUrl}</code>
      </p>
      {state.kind === "checking" && <p>Checking the API...</p>}
      {state.kind === "available" && (
        <p className="healthy">
          {state.health.service} {state.health.apiVersion}: {state.health.status}
        </p>
      )}
      {state.kind === "unavailable" && (
        <p className="unavailable">API unavailable. Start the local API and refresh.</p>
      )}
    </section>
  );
}
