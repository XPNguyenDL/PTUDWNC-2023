export function isEmptyOrSpaces(str) {
  return str === null || (typeof str === "string" && str.match(/^ *$/) !== null);
}

export const API_URL = "https://localhost:7058"

export function isGuid(str) {
  // Regular expression for matching a GUID pattern
  const guidPattern = /^[a-f\d]{8}(-[a-f\d]{4}){3}-[a-f\d]{12}$/i;
  
  // Test the input string against the pattern and return the result
  return guidPattern.test(str);
}