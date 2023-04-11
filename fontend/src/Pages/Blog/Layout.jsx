import React from "react";
import { Outlet } from "react-router-dom";
import { Navbar, Sidebar } from "../../Components/Layouts/";

export default function Layout() {
  return (
    <>
      <Navbar />
      <div className="container-fluid py-3">
        <div className="row">
          <div className="col-9">
            <Outlet />
          </div>
          <div className="col-3 border-start">
            <Sidebar />
          </div>
        </div>
      </div>
    </>
  );
}
