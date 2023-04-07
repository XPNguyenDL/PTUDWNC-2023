import React from 'react'
import PostDetail from '../../Components/PostDetail'
import { useParams } from 'react-router-dom';
import { useState } from 'react';
import { getPostBySlug } from '../../Services/BlogRepository';
import { useEffect } from 'react';
import PostComment from '../../Components/PostComment';
import { getCommentByPost } from '../../Services/CommentRepository';

export default function Post() {
    const params = useParams();
    const [post, setPost] = useState({});
    const [comments, setComments] = useState([]);

    useEffect(() => {
        fetchPosts();
        async function fetchPosts() {
          const data = await getPostBySlug(params.slug);
          if (data) {
            setPost(data);
            const postComments = await getCommentByPost(data.id);
				if (postComments) setComments(postComments);
				else setComments([]);
          } else setPost({});
        }
      }, [params]);

  return (
    <div>
        <PostDetail post={post}/>
        <PostComment comments={comments} postId={post.id}/>
    </div>
  )
}
