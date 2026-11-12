import { ApiStatus } from "@/components/api-status";

export default function Home() {
  return (
    <main>
      <p className="eyebrow">OffshoreMVP</p>
      <h1>Web Intelligence Platform v2</h1>
      <p className="summary">
        The solution foundation is ready. Product modules have not been implemented.
      </p>
      <ApiStatus />
    </main>
  );
}
