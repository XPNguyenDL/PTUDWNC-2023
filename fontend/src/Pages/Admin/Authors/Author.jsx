import React from "react";
import { deleteAuthor, getAuthorsByQueries } from "../../../Services/AuthorRepository";
import { useState } from "react";
import { useEffect } from "react";
import { Loading, PagerAdmin } from "../../../Components";
import { Table } from "react-bootstrap";
import AuthorsFilterPane from "../../../Components/Admin/Authors/AuthorFillterPane";
import { AiOutlineDelete } from "react-icons/ai";
import { Link } from "react-router-dom";

export default function Author() {
  // useState
  const [keyword, setKeyword] = useState("");
  const [authors, setAuthors] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [metadata, setMetadata] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);

  // use Effect
  useEffect(() => {
    fetchPosts();
    async function fetchPosts() {
      const queries = new URLSearchParams({
        PageNumber: pageNumber || 1,
        PageSize: 10,
        SortColumn: "PostCount",
        ...(keyword && { Name: keyword })
      });
      const data = await getAuthorsByQueries(queries);
      if (data) {
        setAuthors(data.items);
        setMetadata(data.metadata);
      } else {
        setAuthors([]);
        setMetadata({});
      }
      setIsLoading(false);
    }
  }, [keyword, pageNumber, authors]);

  // handle
  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
    window.scroll(0, 0);
  };

  const handleDelete = (e, id) => {
    e.preventDefault();
    DeleteAuthor(id);

    async function DeleteAuthor(id) {
      if (window.confirm("Are you sure you want to delete this author?")) {
        const res = await deleteAuthor(id);
        if (res) {
          alert("Delete author successfully");
        } else {
          alert("Delete author failed");
        }
      }
    }
  };

  return (
    <div>
      <h1>Danh sách bài viết</h1>
      <AuthorsFilterPane setKeyword={setKeyword} />
      {isLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Thông tin chi tiết</th>
                <th>Tổng bài viết</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {authors.length > 0 ? (
                authors.map((item) => {
                  return (
                    <tr key={item.id}>
                      <td>
                        <Link
                          to={`/admin/authors/edit/${item.id}`}
                          className="text-bold">
                          {item.fullName}
                        </Link>
                      </td>
                      <td>
                        <p className="text-muted">
                          (<span className="text-muted">{item.postCount}</span>)
                        </p>
                      </td>
                      <td>
                        <div className="d-flex">
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
                      Không có tác giả nào
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
