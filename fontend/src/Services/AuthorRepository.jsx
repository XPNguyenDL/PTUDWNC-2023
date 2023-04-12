import axios from "axios";
import { API_URL } from "../Components/Utils";

export async function getAuthors() {
	const { data } = await axios.get(
		`${API_URL}/api/authors/all`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}