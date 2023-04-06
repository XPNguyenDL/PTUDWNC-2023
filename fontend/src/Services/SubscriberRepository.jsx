import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function subApi(email) {
  try {
    console.log(
      `${API_URL}api/subscribers/${encodeURIComponent(email)}/subscribe`
    );
    const res = await axios.post(
      `${API_URL}/api/subscribers/${encodeURIComponent(email)}/subscribe`
    );

    const data = res.data;
    console.log(data);
    return data;
  } catch (error) {
    console.log(error);
    return null;
  }
}
