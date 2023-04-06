import React from 'react'
import { useState } from 'react';
import { useEffect } from 'react';
import { ListGroup } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { API_URL } from '../Utils';

export default function BestAuthor() {
    const [authors, setAuthors] = useState([]);

    useEffect(() => {
      fetch(`${API_URL}/api/authors/best/4`)
        .then((res) => res.json())
        .then((result) => {
            setAuthors(result.result);
        });
    }, []);
  
    return (
      <div className="mb-4">
        <h3 className="text-success mb-2">Tác giả nổi bật</h3>
        {authors.length > 0 && (
          <ListGroup>
            {authors.map((item) => {
              return (
                <ListGroup.Item key={item.id}>
                  <Link
                    to={`blog/author/${item.urlSlug}`}
                    title={item.urlSlug}
                    key={item.id}>
                    {item.fullName}
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
