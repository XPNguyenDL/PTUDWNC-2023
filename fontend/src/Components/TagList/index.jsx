import React from "react";
import { Link } from "react-router-dom";

export default function TagList({ tagList }) {
  if (!tagList) <></>;

  return (
    <>
      {tagList.map((item, index) => {
        return (
          <Link
            to={`/blog/tag?slug=${item.urlSlug}`}
            title={item.name}
            className="btn btn-sm btn-outline-secondary me-1"
            key={index}>
            {item.name}
          </Link>
        );
      })}
    </>
  );
}
