export type HealthResponse = {
  status: string;
  service: string;
  apiVersion: string;
};

export type TokenResponse = {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAtUtc: string;
  refreshTokenExpiresAtUtc: string;
};

export type ProfileResponse = {
  id: string;
  email: string;
  firstName: string | null;
  lastName: string | null;
  isActive: boolean;
  deactivatedAtUtc: string | null;
};

export type PermissionsResponse = {
  roles: string[];
  modules: Array<{ key: string; name: string; groupKey: string; sortOrder: number }>;
};

export const apiBaseUrl =
  process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5080";

export async function getApiHealth(signal?: AbortSignal): Promise<HealthResponse> {
  const response = await fetch(`${apiBaseUrl}/api/v1/health`, {
    cache: "no-store",
    signal,
  });

  if (!response.ok) {
    throw new Error(`API health request failed with status ${response.status}.`);
  }

  return response.json() as Promise<HealthResponse>;
}

async function request<T>(path: string, init: RequestInit = {}): Promise<T> {
  const response = await fetch(`${apiBaseUrl}/api/v1${path}`, {
    ...init,
    headers: { "Content-Type": "application/json", ...init.headers },
  });
  if (!response.ok) throw new Error(`Request failed with status ${response.status}.`);
  return response.status === 204 ? (undefined as T) : (await response.json() as T);
}

export function login(email: string, password: string): Promise<TokenResponse> {
  return request<TokenResponse>("/auth/login", {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
}

export function refresh(refreshToken: string): Promise<TokenResponse> {
  return request<TokenResponse>("/auth/refresh", {
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });
}

export function getProfile(accessToken: string): Promise<ProfileResponse> {
  return request<ProfileResponse>("/me", { headers: { Authorization: `Bearer ${accessToken}` } });
}

export function getPermissions(accessToken: string): Promise<PermissionsResponse> {
  return request<PermissionsResponse>("/me/permissions", { headers: { Authorization: `Bearer ${accessToken}` } });
}

export function logout(refreshToken: string): Promise<void> {
  return request<void>("/auth/logout", { method: "POST", body: JSON.stringify({ refreshToken }) });
}
