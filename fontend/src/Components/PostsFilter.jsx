import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { Form } from "react-bootstrap";
import { getPostsByQueries } from "../Services/BlogRepository";
import PostItem from "./PostItem";
import Pager from "./Pager";

export default function PostsFilter({ postQuery }) {
  const {
    keyword,
    tagSlug,
    authorSlug,
    categorySlug,
    pageSize,
    sortColumn,
    sortOrder
  } = postQuery;

  const [posts, setPosts] = useState([]);
  const [metadata, setMetadata] = useState({});
  const [pageNumber, setPageNumber] = useState("");

  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
  };

  useEffect(async () => {
    const queries = new URLSearchParams({
      Published: true,
      Unpublished: false,
      PageNumber: pageNumber || 1,
      PageSize: 1
    });
    categorySlug && queries.append("CategorySlug", categorySlug);
    authorSlug && queries.append("AuthorSlug", authorSlug);
    tagSlug && queries.append("TagSlug", tagSlug);
    keyword && queries.append("Keyword", keyword);

    const data = await getPostsByQueries(queries);
    if (data) {
      setPosts(data.items);
      setMetadata(data.metadata);
    } else {
      setPosts([]);
      setMetadata({});
    }
  }, [postQuery]);

  return (
    <div>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
      <Pager metadata={metadata} onPageChange={handleChangePage} />
    </div>
  );
}
