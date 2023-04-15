import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getCommentByPost(id = "") {
  try {
    const res = await axios.get(`${API_URL}/api/comments/${id}/posts`);

    const data = res.data;
    console.log(data);
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    return null;
  }
}

export async function postComment(postId = "", userComment = "", content) {
  try {
    const data = {
      postId: postId,
      userComment: userComment,
      content: content
    };

    let result = {};
    await axios
    .post(`${API_URL}/api/comments`, data)
    .then((response) => {
        // handle success
        result = response
        console.log(response.data);
    })
    .catch((error) => {
        // handle error
        console.error(error);
      });

    if (result) {
      return true;
    } else {
      return false;
    }
  } catch (error) {
    return false;
  }
}
