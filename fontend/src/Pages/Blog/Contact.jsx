import React from "react";

export default function Contact() {
  return (
    <div>
      <div className="p-5">
        <h2 className="mb-5 text-center text-uppercase">Liên hệ</h2>
        <div className="grid">
          <div className="row p-5 border border-1 border-gray rounded">
            <div className="col-4">
              <div class="mb-3 fs-5">Thông tin liên hệ:</div>
              <div>
                <i class="fs-5 fa fa-phone"></i>
                <a href="tel:0123456789" class="ms-1">
                  0123.456.789
                </a>
              </div>
              <div class="my-2">
                <i class="fa fa-envelope"></i>
                <a href="mailto:2014478@dlu.edu.vn" class="ms-1">
                  2014478@dlu.edu.vn
                </a>
              </div>
            </div>
            <div class="col-8">
                <form>
                    <div class="mb-3 fs-3 text-center text-uppercase">Gửi ý kiến</div>
                    <div class="mb-3">
                        <div class="d-flex justify-content-between">
                            <label for="email" class="form-label">Email</label>
                            <span class="text-danger fst-italic fs-6">(Bắt buộc)</span>
                        </div>
                        <input type="email" class="form-control" id="email" name="email" required />
                    </div>
                    <div class="mb-3">
                        <div class="d-flex justify-content-between">
                            <label for="subject" class="form-label">Chủ đề</label>
                            <span class="text-danger fst-italic fs-6">(Bắt buộc)</span>
                        </div>
                        <input type="text" className="form-control" id="subject" name="subject" required />
                    </div>
                    <div class="mb-3">
                        <div class="d-flex justify-content-between">
                            <label for="content" class="form-label">Nội dung</label>
                            <span class="text-danger fst-italic fs-6">(Bắt buộc)</span>
                        </div>
                        <textarea class="form-control" name="body" id="content" rows="8" required></textarea>
                    </div>
                    <div class="d-flex justify-content-between">
                        <button type="submit" class="btn btn-primary px-5 py-2">Gửi</button>
                    </div>
                </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
