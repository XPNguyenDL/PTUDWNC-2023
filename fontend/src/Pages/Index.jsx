import React, { useEffect, useState } from "react";
import PostItem from "../Components/PostItem";
import Pager from "../Components/Pager";
import { useLocation } from "react-router-dom";

export default function Index() {
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
    pageSize = query.get("ps") ?? 1;

  // useEffect
  useEffect(() => {
    document.title = "Trang chủ";
  }, []);

  useEffect(() => {
    fetch(
      `https://localhost:7058/api/posts?Keyword=${keyword}&PageNumber=${pageNumber}&PageSize=${pageSize}&SortColumn=&SortOrder=`
    )
      .then((res) => res.json())
      .then((result) => {
        setPosts(result.result.items);
        console.log(result.result.metadata);
        setMetadata(result.result.metadata)
      });
  }, [keyword, pageNumber, pageSize]);

  if (posts.length < 0 || !posts) {
    console.log(posts);
    return null;
  }

  return (
    <div className="p-4">
      {posts.map((item) => {
        return <PostItem postItem={item} />;
      })}
      <Pager postQuery={{ 'keyword': keyword }} metadata={metadata} />
    </div>
  );
}
