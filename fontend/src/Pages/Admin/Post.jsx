import React from "react";
import { useState } from "react";
import { getPostsByQueries } from "../../Services/BlogRepository";
import { useEffect } from "react";
import { Loading, PagerAdmin, PostsFilterPane } from "../../Components";
import { Table } from "react-bootstrap";
import { Link } from "react-router-dom";
import { AiOutlineEye, AiOutlineEyeInvisible } from "react-icons/ai";

export default function Post() {
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [metadata, setMetadata] = useState({});
  const [keyword, setKeyword] = useState("");
  const [authorSlug, setAuthorSlug] = useState();
  const [categorySlug, setCategorySlug] = useState();

  const handleChangePage = (value) => {
		setPageNumber((current) => current + value);
		window.scroll(0, 0);
	};

  useEffect(() => {
    fetchPosts();
    async function fetchPosts() {
      const queries = new URLSearchParams({
        PageNumber: pageNumber || 1,
        PageSize: 10,
        ...(keyword && { Keyword: keyword }),
        ...(authorSlug && { AuthorSlug: authorSlug }),
        ...(categorySlug && { CategorySlug: categorySlug }),
      });
      const data = await getPostsByQueries(queries);
      if (data) {
        setPosts(data.items);
        setMetadata(data.metadata);
      } else {
        setPosts([]);
        setMetadata({});
      }
      setIsLoading(false);
    }
  }, [keyword, authorSlug, categorySlug, pageNumber]);

  return (
    <div>
      <h1>Danh sách bài viết</h1>
      <PostsFilterPane
        setKeyword={setKeyword}
        setAuthorSlug={setAuthorSlug}
        setCategorySlug={setCategorySlug}
      />
      {isLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Tiêu đề</th>
                <th>Tác giả</th>
                <th>Chủ đề</th>
                <th>Xuất bản</th>
              </tr>
            </thead>
            <tbody>
              {posts.length > 0 ? (
                posts.map((item) => {
                  return (
                    <tr key={item.id}>
                      <td>
                        <Link
                          to={`admin/posts/edit/${item.id}`}
                          className="text-bold">
                          {item.title}
                        </Link>
                        <p className="text-muted">{item.shortDescription}</p>
                      </td>
                      <td>{item.author.fullName}</td>
                      <td>{item.category.name}</td>
                      <td>
                        {item.published ? (
                          <AiOutlineEye />
                        ) : (
                          <AiOutlineEyeInvisible />
                        )}
                      </td>
                    </tr>
                  );
                })
              ) : (
                <tr>
                  <td colSpan={4}>
                    <h4 className="text-danger text-center">
                      Không có bài viết nào
                    </h4>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
          <PagerAdmin metadata={metadata} onPageChange={handleChangePage} />
        </>
      )}
    </div>
  );
}
