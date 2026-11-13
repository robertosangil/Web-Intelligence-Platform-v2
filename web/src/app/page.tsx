import { ApiStatus } from "@/components/api-status";
import { AccessPanel } from "@/components/access-panel";

export default function Home() {
  return (
    <main>
      <p className="eyebrow">OffshoreMVP</p>
      <h1>Web Intelligence Platform v2</h1>
      <p className="summary">
        The solution foundation is ready. Product modules have not been implemented.
      </p>
      <ApiStatus />
      <AccessPanel />
    </main>
  );
}
