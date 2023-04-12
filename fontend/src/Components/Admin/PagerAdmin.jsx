import React from 'react'
import { Button } from 'react-bootstrap';

export default function PagerAdmin({ metadata, onPageChange }) {
  // Component's props
	const { pageCount, hasNextPage, hasPreviousPage } = metadata;

	return (
		<>
			{pageCount > 1 && (
				<div className='my-4 text-center'>
					{hasPreviousPage ? (
						<button
							className='btn btn-info'
							onClick={() => onPageChange(-1)}
						>
							&nbsp;Trang trước
						</button>
					) : (
						<Button variant='outline-secondary' disabled>
							&nbsp;Trang trước
						</Button>
					)}
					{hasNextPage ? (
						<button
							className='btn btn-info ms-1'
							onClick={() => onPageChange(1)}
						>
							Trang sau&nbsp;
						</button>
					) : (
						<Button
							variant='outline-secondary'
							className='ms-1'
							disabled
						>
							Trang sau&nbsp;
						</Button>
					)}
				</div>
			)}
		</>
	);
}
