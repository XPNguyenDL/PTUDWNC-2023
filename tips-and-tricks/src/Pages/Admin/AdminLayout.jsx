import React from "react";
import { NavbarAdmin } from "../../Components/Admin";
import { Outlet } from "react-router-dom";

export default function AdminLayout() {
  return (
    <div>
      <NavbarAdmin />
      <div className="container-fluid py-3">
        <Outlet />
      </div>
    </div>
  );
}
