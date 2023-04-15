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

export async function getPost(postQuery) {
  try {
    const {
      keyword,
      tagSlug,
      authorSlug,
      categorySlug,
      pageNumber,
      pageSize,
    } = postQuery;

    const parameters = new URLSearchParams({
      keyword: keyword || "",
      authorSlug: authorSlug || "",
      tagSlug: tagSlug || "",
      categorySlug: categorySlug || "",
      pageNumber: pageNumber || 1,
      pageSize: pageSize || 10,
    });

    const res = await axios.get(`${API_URL}/api/posts?${parameters}`);

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

export async function getPostsByQueries(parameters) {
  try {
    const res = await axios.get(`${API_URL}/api/posts?${parameters}`);

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

export async function getPostBySlug(slug = "") {
  try {
    const res = await axios.get(`${API_URL}/api/posts/byslug/${slug}`);

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

export async function addPost(post) {
  try {
    const res = await axios.post(`${API_URL}/api/posts`, post);
    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      console.log(data);
      return null;
    }
  } catch (error) {
    console.log('Error', error);
    return null;
  }
}

export async function updatePost(postId, post) {
  try {
    // const res = await axios.put(`${process.env.REACT_APP_API_URL}/api/posts/${postId}`, post);
    const res = await axios.put(`${API_URL}/api/posts/${postId}`, post);
    const data = res.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    console.log('Error', error);
    return null;
  }
}

export async function updatePostPicture(postId, image) {
  try {
    const formData = new FormData();
    formData.append("file", image);
    const res = await axios.post(`${API_URL}/api/posts/${postId}/picture`, formData);

    const data = res.data;
    if (data.isSuccess) {
      return data;
    } else {
      return null;
    }
  } catch (error) {
    console.log('Error', error);
    return null;
  }
}

export async function getTogglePost(id) {
  try {
    const res = await axios.get(`${API_URL}/api/posts/TogglePost/${id}`);

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

export async function deletePost(id) {
  try {
    const res = await axios.delete(`${API_URL}/api/posts/${id}`);

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