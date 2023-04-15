import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { Pager, PostItem } from "../../Components";
import { getPost } from "../../Services/BlogRepository";

export default function Home() {
  // useState()
  const [posts, setPosts] = useState([]);
  const [metadata, setMetadata] = useState({});
  function useQuery() {
    const { search } = useLocation();
    return React.useMemo(() => new URLSearchParams(search), [search]);
  }

  const query = useQuery(),
    keyword = query.get("k") ?? "",
    pageNumber = query.get("p") ?? 1,
    pageSize = query.get("ps") ?? 10;

  // useEffect
  useEffect(() => {
    document.title = "Trang chủ";
  }, []);

  useEffect(() => {
    fetchPosts();
    async function fetchPosts() {
      const postQuery = {
        keyword: keyword,
        pageSize: pageSize,
        pageNumber: pageNumber
      }
      const data = await getPost(postQuery);
      if (data) {
        setPosts(data.items);
        setMetadata(data.metadata)
      } else {
        setPosts([]);
      }
    }
  }, [keyword, pageNumber, pageSize]);

  return (
    <div className="p-4">
      {posts.map((item) => {
        return <PostItem postItem={item} />;
      })}
      <Pager postQuery={keyword} metadata={metadata} />
    </div>
  );
}
