import React from 'react'
import { useState, useEffect } from 'react';
import { API_URL } from '../Utils';
import { ListGroup } from 'react-bootstrap';
import { Link } from 'react-router-dom';

export default function FeaturedPosts() {
    const [post, setPost] = useState([]);

    useEffect(() => {
      fetch(`${API_URL}/api/posts/featured/3`)
        .then((res) => res.json())
        .then((result) => {
            setPost(result.result);
        });
    }, []);
  
    return (
      <div className="mb-4">
        <h3 className="text-success mb-2">Bài viết nổi bật</h3>
        {post.length > 0 && (
          <ListGroup>
            {post.map((item) => {
              return (
                <ListGroup.Item key={item.id}>
                  <Link
                    to={`blog/post/${item.urlSlug}`}
                    title={item.shortDescription}
                    key={item.id}>
                    {item.title}
                    <span>&nbsp;({item.viewCount})</span>
                  </Link>
                </ListGroup.Item>
              );
            })}
          </ListGroup>
        )}
      </div>
    );
}
