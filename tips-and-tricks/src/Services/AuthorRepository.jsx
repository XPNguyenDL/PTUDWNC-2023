import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getAuthors() {
  const { data } = await axios.get(`${API_URL}/api/authors/all`);

  if (data.isSuccess) return data.result;
  else return null;
}

export async function getAuthorsByQueries(parameters) {
  try {
    const res = await axios.get(`${API_URL}/api/authors?${parameters}`);

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

export async function deleteAuthor(id) {
  try {
    const res = await axios.delete(`${API_URL}/api/authors/${id}`);

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

export async function addAuthor(author) {
  try {
    const res = await axios.post(`${API_URL}/api/authors`, author);
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

export async function updateAuthor(authorId, author) {
  try {
    const res = await axios.put(`${API_URL}/api/authors/${authorId}`, author);
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

export async function getAuthorById(id) {
  try {
    const reponse = await axios.get(`${API_URL}/api/authors/${id}`);
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
