export default function isEmptyOrSpaces(str) {
  return str === null || (typeof str === "string" && str.match(/^ *$/) !== null);
}

export const API_URL = "https://localhost:7058"

