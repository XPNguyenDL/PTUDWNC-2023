import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getPostById(id) {
  try {
    const reponse = await axios.get(`${API_URL}/api/posts/${id}`);
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

export async function getPost(
  keyword = "",
  pageSize = 10,
  pageNumber = 1,
  sortColumn = "",
  sortOrder = ""
) {
  try {
    const res = await axios.get(
      `${API_URL}/api/posts?Keyword=${keyword}&PageNumber=${pageNumber}&PageSize=${pageSize}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
    );

    const data = res.data;
    if (data.isSuccess) {
      return data.results;
    } else {
      return null;
    }
  } catch (error) {
    return null;
  }
}

export async function getPostByAuthor(
  slug = "",
  pageSize = 10,
  pageNumber = 1
) {
  try {
    const res = await axios.get(
      `${API_URL}/api/authors/${slug}/posts?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );

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

export async function getPostByCategory(
  slug = "",
  pageSize = 10,
  pageNumber = 1
) {
  try {
    const res = await axios.get(
      `${API_URL}/api/categories/${slug}/posts?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );

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
