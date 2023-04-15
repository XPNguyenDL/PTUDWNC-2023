import React from 'react'
import SearchForm from '../SearchForm'
import CategoriesWidget from '../Widget/CategoriesWidget'
import FeaturedPosts from '../Widget/FeaturedPosts'
import RandomPosts from '../Widget/RandomPosts'
import TagCloud from '../Widget/TagCloud'
import BestAuthor from '../Widget/BestAuthor'
import Archives from '../Widget/Archives'
import { Link } from 'react-router-dom'

export default function Sidebar() {
  return (
    <div className='pt-4 ps-2'>
        <SearchForm />
        <CategoriesWidget />
        <FeaturedPosts />
        <RandomPosts />
        <TagCloud />
        <BestAuthor />
        <Archives />
        <Link 
        to={`blog/subscriber`}
        className='btn btn-info'>
            Đăng ký nhận tin mới
        </Link>
        <h1>
            Tag cloud
        </h1>
    </div>
  )
}
