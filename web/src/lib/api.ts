export type HealthResponse = {
  status: string;
  service: string;
  apiVersion: string;
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
