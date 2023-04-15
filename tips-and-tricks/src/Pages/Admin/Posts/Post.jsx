import React, { useState, useEffect } from "react";
import {
  deletePost,
  getPostsByQueries,
  getTogglePost
} from "../../../Services/BlogRepository";
import { Loading, PagerAdmin, PostsFilterPane } from "../../../Components";
import { Table } from "react-bootstrap";
import { Link } from "react-router-dom";
import {
  AiOutlineDelete,
  AiOutlineEye,
  AiOutlineEyeInvisible
} from "react-icons/ai";

export default function Post() {
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [metadata, setMetadata] = useState({});
  const [keyword, setKeyword] = useState("");
  const [authorSlug, setAuthorSlug] = useState();
  const [categorySlug, setCategorySlug] = useState();
  
  // handle
  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
    window.scroll(0, 0);
  };

  const handleTogglePost = (e, id) => {
    e.preventDefault();
    TogglePost(id);

    async function TogglePost(id) {
      const res = await getTogglePost(id);
      if (res) {
        alert(res);
      } else {
        alert("Toggle post is unsucceeded");
      }
    }
  };

  const handleDelete = (e, id) => {
    e.preventDefault();
    DeletePost(id);

    async function DeletePost(id) {
      if (window.confirm("Are you sure you want to delete this post?")) {
        const res = await deletePost(id);
        if (res) {
          alert("Delete post successfully")
        } else {
          alert("Delete post failed")
        }
      }
    }
  };
  useEffect(() => {
    fetchPosts();
    async function fetchPosts() {
      const queries = new URLSearchParams({
        PageNumber: pageNumber || 1,
        PageSize: 10,
        ...(keyword && { Keyword: keyword }),
        ...(authorSlug && { AuthorSlug: authorSlug }),
        ...(categorySlug && { CategorySlug: categorySlug })
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
  }, [keyword, authorSlug, categorySlug, pageNumber, posts]);

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
                          to={`/admin/posts/edit/${item.id}`}
                          className="text-bold">
                          {item.title}
                        </Link>
                        <p className="text-muted">{item.shortDescription}</p>
                      </td>
                      <td>{item.author.fullName}</td>
                      <td>{item.category.name}</td>
                      <td>
                        <div className="d-flex">
                          <div onClick={(e) => handleTogglePost(e, item.id)}>
                            {item.published ? (
                              <AiOutlineEye />
                            ) : (
                              <AiOutlineEyeInvisible />
                            )}
                          </div>
                          <div
                            className="text-danger ms-2"
                            onClick={(e) => handleDelete(e, item.id)}>
                            <AiOutlineDelete />
                          </div>
                        </div>
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
