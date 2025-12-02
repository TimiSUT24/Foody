import { useEffect } from "react";

export default function KlarnaExpress({ client_token,session_id, orderPayload }) {
  useEffect(() => {
    const interval = setInterval(() => {
      if (window.initKlarnaButtons && client_token && orderPayload) {
        window.initKlarnaButtons(client_token, orderPayload);
        clearInterval(interval);
      }
    }, 100); // polls until script is loaded

    return () => clearInterval(interval);
  }, [client_token, session_id, orderPayload]);

  return <div id="klarna-express-container" style={{ width: "100%" }}></div>;
}
