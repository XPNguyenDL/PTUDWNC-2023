import axios from "axios";

export async function getPostById(id) {
  try {
    const reponse = await axios.get(`https://localhost:7058/api/posts/${id}`);
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
      `https://localhost:7058/api/posts?Keyword=${keyword}&PageNumber=${pageNumber}&PageSize=${pageSize}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
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
