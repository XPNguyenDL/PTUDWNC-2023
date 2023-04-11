import React from "react";
import { useRef, useState } from "react";
import { subApi } from "../../Services/SubscriberRepository";

export default function Subscriber() {
  const email = useRef();
  const [result, setResult] = useState({});

  const handleSubmit = async (e) => {
    e.preventDefault();

    const data = await subApi(email.current.value);
    if (data !== null) {
      setResult(data);
    } else setResult({});
  };

  return (
    <div
      className="mx-auto mt-5 p-5 rounded shadow"
      style={{ maxWidth: "576px" }}>
      <h5 className="text-uppercase">Đăng ký nhận thông báo</h5>
      <p>Nhận thông báo về tin tức, bài viết mới thông qua email.</p>
      <div className="mb-4">
        <form onSubmit={handleSubmit}>
          <div className="input-group mb-3">
            <input
              type="text"
              className="form-control"
              name="email"
              required
              placeholder="Enter email"
              aria-label="Enter email"
              ref={email}
            />
            <button className="btn btn-outline-secondary" type="submit">
              Đăng ký
            </button>
          </div>
        </form>
        {result && (
          <span className={result.isSuccess ? "text-success" : "text-danger"}>

            {result.isSuccess ? result.result : result.errors[0]}
          </span>
        )}
      </div>
      <a asp-controller="Newsletter" asp-action="UnSubscribe">
        Hủy đăng ký
      </a>
    </div>
  );
}
