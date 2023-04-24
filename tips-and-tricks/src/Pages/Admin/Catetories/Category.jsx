import React from "react";
import CategoriesFilterPane from "../../../Components/Admin/Categories/CategoriesFilterPane";
import { useState } from "react";
import { Link } from "react-router-dom";
import { Table } from "react-bootstrap";
import { Loading, PagerAdmin } from "../../../Components";
import { AiOutlineDelete } from "react-icons/ai";
import {
  deleteCategory,
  getCategoriesByQueries
} from "../../../Services/CategoryRepository";
import { useEffect } from "react";

export default function Category() {
  // useState
  const [keyword, setKeyword] = useState("");
  const [categories, setCategories] = useState([]);
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
      const data = await getCategoriesByQueries(queries);
      if (data) {
        setCategories(data.items);
        setMetadata(data.metadata);
      } else {
        setCategories([]);
        setMetadata({});
      }
      setIsLoading(false);
    }
  }, [keyword, pageNumber, categories]);

  // handle
  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
    window.scroll(0, 0);
  };

  const handleDelete = (e, id) => {
    e.preventDefault();
    DeleteCategory(id);

    async function DeleteCategory(id) {
      if (window.confirm("Are you sure you want to delete this category?")) {
        const res = await deleteCategory(id);
        if (res) {
          alert("Delete category successfully");
        } else {
          alert("Delete category failed");
        }
      }
    }
  };

  return (
    <div>
      <h1>Danh sách chủ đề</h1>
      <CategoriesFilterPane setKeyword={setKeyword} />
      {isLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Thông tin chi tiết</th>
                <th>Tổng bài viết</th>
                <th>Xuất bản</th>
              </tr>
            </thead>
            <tbody>
              {categories.length > 0 ? (
                categories.map((item) => {
                  return (
                    <tr key={item.id}>
                      <td>
                        <Link
                          to={`/admin/categories/edit/${item.id}`}
                          className="text-bold">
                          {item.name}
                        </Link>
                      </td>
                      <td>
                        <p className="text-muted">
                          {item.description} (
                          <span className="text-muted">{item.postCount}</span>)
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
                      Không có chủ đề nào
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
