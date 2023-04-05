import React, { useEffect, useState } from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function CategoriesWidget() {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    fetch(`https://localhost:7058/api/categories?PageNumber=1&PageSize=5&SortColumn=PostCount&SortOrder=DESC`)
      .then((res) => res.json())
      .then((result) => {
        console.log(result);
        setCategories(result.result.items);
      });
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Các chủ đề</h3>
      {categories.length > 0 && (
        <ListGroup>
          {categories.map((item) => {
            return (
              <ListGroup.Item key={item.id}>
                <Link
                  to={`blog/category?slug=${item.urlSlug}`}
                  title={item.description}
                  key={item.id}>
                  {item.name}
                  <span>&nbsp;({item.postCount})</span>
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </div>
  );
}
