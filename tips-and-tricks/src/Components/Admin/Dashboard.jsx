import React, { useEffect, useState } from "react";
import { getDashboard } from "../../Services/DashboardRespository";

export default function Dashboard() {
  const [data, setData] = useState({});

  useEffect(() => {
    fetchData();

    async function fetchData() {
      const res = await getDashboard();
      if (res) {
        setData(res);
      } else {
        setData({})
      }
    }
  }, []);

  return (
    <div className="row mt-2">
      <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
          <div className="card-body d-flex justify-content-between">
            <h5 className="card-title">
              Bài viết <span>| Hiện tại</span>
            </h5>
            <div className="d-flex align-items-center">
              <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                <i className="bi bi-cart"></i>
              </div>
              <div className="ps-3">
                <h6>{data.postCount}</h6>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Bài viết <span>| Chưa xuất bản</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.postUnPublicCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Tác giả <span>| Hiện tại</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.authorCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Bình luận <span>| Hiện tại</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.cmtCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Bình luận <span>| Chưa duyệt</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.cmtNotVerifyCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Đăng ký <span>| Hiện tại</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.subCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div className="col-xxl-3 col-md-4 mt-2">
        <div className="card info-card sales-card ">
            <div className="card-body d-flex justify-content-between">
                <h5 className="card-title">Đăng ký <span>| Hôm nay</span></h5>
                <div className="d-flex align-items-center">
                    <div className="card-icon rounded-circle d-flex align-items-center justify-content-center">
                        <i className="bi bi-cart"></i>
                    </div>
                    <div className="ps-3">
                        <h6>{data.subDailyCount}</h6>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
  );
}
