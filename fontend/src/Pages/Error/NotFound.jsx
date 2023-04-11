import React from "react";
import { Link } from "react-router-dom";

export default function NotFound() {
  return (
    <div className="d-flex flex-column container mx-auto align-items-center">
      <div className="my-5">
        <img
          src="https://www.pngitem.com/pimgs/m/561-5616833_image-not-found-png-not-found-404-png.png"
          alt="not-found"
        />
      </div>
      <div className="mb-5">
        <Link to="/" className="btn btn-warning">
          Về trang chủ
        </Link>
      </div>
    </div>
  );
}
