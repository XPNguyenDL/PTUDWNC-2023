import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getCategories() {
  const { data } = await axios.get(`${API_URL}/api/categories/all`);

  if (data.isSuccess) return data.result;
  else return null;
}

export async function getCategoriesByQueries(parameters) {
  try {
    const res = await axios.get(`${API_URL}/api/categories?${parameters}`);

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

export async function deleteCategory(id) {
  try {
    const res = await axios.delete(`${API_URL}/api/categories/${id}`);

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

export async function addCategory(category) {
  try {
    const res = await axios.post(`${API_URL}/api/categories`, category);
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

export async function updateCategory(categoryId, category) {
  try {
    const res = await axios.put(`${API_URL}/api/categories/${categoryId}`, category);
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

export async function getCategoryById(id) {
	try {
		const reponse = await axios.get(`${API_URL}/api/categories/${id}`);
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
