import React from "react";
import {
  deleteComment,
  getCommentByQueries,
  toggleComment
} from "../../Services/CommentRepository";
import { useState } from "react";
import { useEffect } from "react";
import { Button, Table } from "react-bootstrap";
import { Loading, PagerAdmin } from "../../Components";
import { Link } from "react-router-dom";
import { AiOutlineDelete } from "react-icons/ai";
import CommentsFilterPane from "../../Components/Admin/Comments/CommentsFilterPane";

export default function Comment() {
  // useState
  const [keyword, setKeyword] = useState("");
  const [comments, setComments] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [metadata, setMetadata] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);

  // use Effect
  useEffect(() => {
    fetchComment();
    async function fetchComment() {
      const queries = new URLSearchParams({
        PageNumber: pageNumber || 1,
        PageSize: 10,
        ...(keyword && { Keyword: keyword })
      });
      const data = await getCommentByQueries(queries);
      if (data) {
        setComments(data.items);
        setMetadata(data.metadata);
      } else {
        setComments([]);
        setMetadata({});
      }
      setIsLoading(false);
    }
  }, [keyword, pageNumber, comments]);

  // handle
  const handleChangePage = (value) => {
    setPageNumber((current) => current + value);
    window.scroll(0, 0);
  };

  const handleDelete = (e, id) => {
    e.preventDefault();
    DeleteComment(id);

    async function DeleteComment(id) {
      if (window.confirm("Are you sure you want to delete this comment?")) {
        const res = await deleteComment(id);
        if (res) {
          alert("Delete comment successfully");
        } else {
          alert("Delete comment failed");
        }
      }
    }
  };

  const handleToggle = (e, id) => {
    e.preventDefault();
    ToogleComment(id);

    async function ToogleComment(id) {
      if (window.confirm("Are you sure?")) {
        const res = await toggleComment(id);
        if (res) {
          alert("successfully");
        } else {
          alert("failed");
        }
      }
    }
  };

  return (
    <div>
      <h1>Danh sách bình luận</h1>
      <CommentsFilterPane setKeyword={setKeyword} />
      {isLoading ? (
        <Loading />
      ) : (
        <>
          <Table striped responsive bordered>
            <thead>
              <tr>
                <th>Thông tin chi tiết</th>
                <th>Trạng thái</th>
                <th></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {comments.length > 0 ? (
                comments.map((item) => {
                  return (
                    <tr key={item.id}>
                      <td>
                        <div>Tên: {item.userComment}</div>
                        <div>
                          Nội dung:{" "}
                          <span className="text-muted">{item.content}</span>
                        </div>
                      </td>
                      <td>
                        {item.commentStatus === 2 ? (
                          <span>Hợp lệ</span>
                        ) : (
                          <span>Chưa phê duyệt</span>
                        )}
                      </td>
                      <td>
                        {item.commentStatus === 2 ? (
                          <Button
                            className="btn-success"
                            disabled>
                            Đã phê duyệt
                          </Button>
                        ) : (
                          <Button
                            onClick={(e) => handleToggle(e, item.id)}
                            className="btn-info">
                            Phê duyệt
                          </Button>
                        )}
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
                      Không có bình luận nào
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
