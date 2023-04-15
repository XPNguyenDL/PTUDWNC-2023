import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getTags() {
  const { data } = await axios.get(`${API_URL}/api/tags/all`);

  if (data.isSuccess) return data.result;
  else return null;
}

export async function getTagsByQueries(parameters) {
  try {
    const res = await axios.get(`${API_URL}/api/tags?${parameters}`);

    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    return null;
  }
}

export async function deleteTag(id) {
  try {
    const res = await axios.delete(`${API_URL}/api/tags/${id}`);

    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    return null;
  }
}

export async function addTag(tag) {
  try {
    const res = await axios.post(`${API_URL}/api/tags`, tag);
    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      console.log(data);
      return null;
    }
  } catch (error) {
    console.log("Error", error);
    return null;
  }
}

export async function updateTag(tagId, tag) {
  try {
    const res = await axios.put(`${API_URL}/api/tags/${tagId}`, tag);
    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    console.log("Error", error);
    return null;
  }
}

export async function getTagById(id) {
	try {
		const reponse = await axios.get(`${API_URL}/api/tags/${id}`);
		const data = reponse.data;
	
		if (data.isSuccess) {
		  return data.result;
		} else {
		  return null;
		}
	  } catch (error) {
		console.log("Error", error.message);
		return null;
	  }
}
