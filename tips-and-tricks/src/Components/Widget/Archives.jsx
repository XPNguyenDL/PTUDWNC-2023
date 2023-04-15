import React from "react";
import { useState } from "react";
import { API_URL } from "../Utils";
import { useEffect } from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function Archives() {
  const [archives, setArchives] = useState([]);

  useEffect(() => {
    fetch(`${API_URL}/api/posts/archives/12`)
      .then((res) => res.json())
      .then((result) => {
        setArchives(result.result);
      });
  }, []);


  function getMonthName(monthNumber) {
    const date = new Date();
    date.setMonth(monthNumber - 1);
  
    return date.toLocaleString('en-US', {
      month: 'long',
    });
  }
  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Bài viết theo tháng</h3>
      {archives.length > 0 && (
        <ListGroup>
          {archives.map((item, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link to={`/blog/archive/${item.year}/${item.month}`}>
                  {`${getMonthName(item.month)} ${item.year} (${item.postCount})`}
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </div>
  );
}
