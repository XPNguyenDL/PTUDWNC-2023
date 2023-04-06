import React from "react";
import { useRef } from "react";
import { Button, Form } from "react-bootstrap";
import { AiOutlineSearch } from "react-icons/ai";

export default function SearchForm() {
    const keyword = useRef("");
  
    const handleSubmit = (e) => {
      e.preventDefault();
      window.location = `/blog?k=${keyword.current.value}`;
    };
  
    return (
      <div>
        <Form method="get" onSubmit={handleSubmit}>
          <Form.Group>
            <Form.Control
              type="text"
              name="k"
              aria-label="Enter keyword"
              aria-describedby="btnSearchPost"
              placeholder="Enter keyword"
              ref={keyword}
            />
            <Button id="btnSearchPost" variant="outline-secondary" type="submit">
              <AiOutlineSearch />
            </Button>
          </Form.Group>
        </Form>
      </div>
    );
  }
  