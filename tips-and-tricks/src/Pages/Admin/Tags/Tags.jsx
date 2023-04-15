import React, { useEffect, useState } from "react";
import { AiOutlineDelete } from "react-icons/ai";
import { Loading, PagerAdmin, TagsFilter } from "../../../Components";
import { Link } from "react-router-dom";
import { Table } from "react-bootstrap";
import { deleteTag, getTagsByQueries } from "../../../Services/TagsRepository";

export default function Tags() {
  // useState
  const [keyword, setKeyword] = useState("");
  const [tags, setTags] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [metadata, setMetadata] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);

  // use Effect
  useEffect(() => {
    fetchTags();
    async function fetchTags() {
      const queries = new URLSearchParams({
        PageNumber: pageNumber || 1,
        PageSize: 10,
        SortColumn: "PostCount",
        ...(keyword && { Name: keyword })
      });
      const data = await getTagsByQueries(queries);
      if (data) {
        setTags(data.items);
        setMetadata(data.metadata);
      } else {
        setTags([]);
        setMetadata({});
      }
      setIsLoading(false);
    }
  }, [keyword, pageNumber]);

  // handle
  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
    window.scroll(0, 0);
  };

  const handleDelete = (e, id) => {
    e.preventDefault();
    DeleteTags(id);

    async function DeleteTags(id) {
      if (window.confirm("Are you sure you want to delete this category?")) {
        const res = await deleteTag(id);
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
      <h1>Danh sách bài viết</h1>
      <TagsFilter setKeyword={setKeyword} />
      {isLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Tên thẻ</th>
                <th>Thông tin chi tiết</th>
                <th>Tổng bài viết</th>
                <th>Xuất bản</th>
              </tr>
            </thead>
            <tbody>
              {tags.length > 0 ? (
                tags.map((item) => {
                  return (
                    <tr key={item.id}>
                      <td>
                        <Link
                          to={`/admin/tags/edit/${item.id}`}
                          className="text-bold">
                          {item.name}
                        </Link>
                      </td>
                      <td>{item.description}</td>
                      <td>
                        (<span className="text-muted">{item.postCount}</span>)
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
                      Không có thẻ nào
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
