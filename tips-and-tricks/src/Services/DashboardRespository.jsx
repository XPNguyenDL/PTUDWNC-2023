import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getDashboard() {
    try {
      const res = await axios.get(
        `${API_URL}/api/dashboards`
      );
  
      const data = res.data;
      if (data.isSuccess) {
          return data.result;
      } else {
        return null;
      }
    } catch (error) {
      console.log(error);
      return null;
    }
  }