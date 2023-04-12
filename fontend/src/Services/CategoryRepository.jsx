import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getCategories() {
	const { data } = await axios.get(
		`${API_URL}/api/categories/all`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}