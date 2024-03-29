import React from "react";
import { useRef } from "react";
import { postComment } from "../Services/CommentRepository";
import { useState } from "react";
import { useEffect } from "react";

export default function PostComment({ comments, postId }) {
  const userName = useRef();
  const content = useRef();

  const [isSuccess, setIsSuccess] = useState(false);

  const date = (date) => new Date(date).toLocaleDateString("en-GB");

  const handleSendComment = async (e) => {
    e.preventDefault();
    console.log(postId);
    const data = await postComment(
      postId,
      userName.current.value,
      content.current.value
    );
    if (data !== null) {
      setIsSuccess(data);
    } else setIsSuccess(false);
  };

  return (
    <div>
      <section className="mb-5">
        <div className="card bg-light">
          <div className="card-body">
            <form className="mb-4" onSubmit={handleSendComment}>
              <div className="input-group mb-3">
                <input
                  type="text"
                  className="form-control"
                  name="userName"
                  required
                  placeholder="Enter user name..."
                  aria-label="Enter user name..."
                  aria-describedby="btnComment"
                  ref={userName}
                />

                <button
                  className="btn btn-outline-secondary"
                  type="submit"
                  id="btnComment">
                  Đăng bình luận
                </button>
              </div>
              <textarea
                className="form-control"
                rows="3"
                placeholder="Enter content..."
                name="content"
                ref={content}
                required
                aria-describedby="btnComment"></textarea>
            </form>
            {isSuccess && (
              <div>
                <span className="text-success">Gửi thành công</span>
              </div>
            )}
            {comments.map((item) => {
              return (
                <div className="d-flex mb-2">
                  <div className="flex-shrink-0 mr-4">
                    <img
                      className="rounded-circle"
                      src="https://dummyimage.com/50x50/ced4da/6c757d.jpg"
                      alt="..."
                    />
                  </div>
                  <div className="ms-3">
                    <div className="fw-bold">
                      {item.userComment}
                      <small className="text-light">
                        {date(item.PostTime)}
                      </small>
                    </div>
                    {item.content}
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </section>
    </div>
  );
}
