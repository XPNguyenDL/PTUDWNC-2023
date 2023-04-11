import React from "react";

export default function BadRequest() {
  return (
    <div className="d-flex flex-column container mx-auto align-items-center">
      <div className="my-5">
        <h1 className="fs-1 text-danger">404</h1>
      </div>
      <div className="mb-5">
        <p className="fs-3">Không tìm thấy trang</p>
      </div>
      <div className="mb-5">
        <Link to="/" className="btn btn-warning">
          Về trang chủ
        </Link>
      </div>
    </div>
  );
}
