import React from "react";
import { API_URL } from "./Utils";

export default function PostDetail({ post }) {
  const date = new Date(post.postedDate).toLocaleDateString("en-GB");

  return (
    <div>
      <div class="d-flex justify-content-between">
        <header class="mb-4">
          <p class="fw-bolder mb-2 text-dark fs-5">{post.title}</p>
          <partial name="_TagList" model="@Model.Tags" />
          <div class="text-muted fst-italic mb-2">{date}</div>
        </header>
        <div
          class="fb-share-button mt-4"
          data-href="@linkShare"
          data-layout="button_count"
          data-size="large">
          <a
            target="_blank"
            href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Flocalhost%3A7245%2Fblog%2Fpost%2F2022%2F9%2F30%2F10-web-development-trends-in-2023&amp;src=sdkpreparse"
            class="fb-xfbml-parse-ignore">
            Chia sẻ
          </a>
        </div>
      </div>
      <figure class="mb-4">
      {post.imageUrl && (
					<img
          class="img-fluid rounded mx-auto d-block h-512 object-fit-cover"
          src={`${API_URL}/${post.imageUrl}`}
          alt="..."
        />
				)}
        
      </figure>
      <section class="mb-5">
        <small class="fs-6 mb-4">{post.description}</small>
      </section>
      
    </div>
  );
}
