import React from "react";
import { useLocation, useParams } from "react-router-dom";
import { getPostByAuthor } from "../../Services/BlogRepository";
import { PostItem, Pager } from "../../Components/";
import { useState, useEffect } from "react";

export default function PostByAuthor() {
  const params = useParams();
  const [posts, setPosts] = useState([]);
  const [metadata, setMetadata] = useState({});

  function useQuery() {
    const { search } = useLocation();
    return React.useMemo(() => new URLSearchParams(search), [search]);
  }

  const query = useQuery(),
    pageNumber = query.get("p") ?? 1,
    pageSize = query.get("ps") ?? 10;

  useEffect(() => {
    fetchPostsByAuthorSlug();
    async function fetchPostsByAuthorSlug() {
      const data = await getPostByAuthor(params.slug, pageSize, pageNumber);
      if (data) {
        setPosts(data.items);
        setMetadata(data.metadata);
      } else setPosts([]);
    }
  }, [params, pageSize, pageNumber]);

  return (
    <div className="p-4">
      {posts.map((item) => {
        return <PostItem key={item.id} postItem={item} />;
      })}
      <Pager postQuery="" metadata={metadata} />
    </div>
  );
}
