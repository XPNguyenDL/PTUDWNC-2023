import React from 'react'
import { API_URL } from '../Utils';
import { useState } from 'react';
import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

export default function TagCloud() {
    const [tagCloud, setTagCloud] = useState([]);

    useEffect(() => {
      fetch(`${API_URL}/api/tags?PageNumber=1&PageSize=10`)
        .then((res) => res.json())
        .then((result) => {
            setTagCloud(result.result.items);
        });
    }, []);
  
    return (
      <div className="mb-4">
        <h3 className="text-success mb-2">Từ khóa</h3>
        {tagCloud.length > 0 && (
          <ListGroup >
            {tagCloud.map((item) => {
              return (
                <ListGroup.Item key={item.id} 
                className='btn btn-outline-secondary mt-2'>
                  <Link
                    to={`blog/post/${item.urlSlug}`}
                    title={item.description}
                    className='text-black'
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
