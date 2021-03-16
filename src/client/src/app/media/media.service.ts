import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { Media } from './models/Media.type';
import { UploadResult } from './models/UploadResult.type';

@Injectable({
	providedIn: 'root',
})
export class MediaService {
	constructor(private http: HttpClient) {}

	public getAllMedia(useCache = true): Promise<Media[]> {
		return this.http.get<Media[]>(`${env.api.url}/media`, {
			params: {
				useCache: String(useCache)
			}
		}).toPromise();
	}

	public uploadFiles(fileList: FileList): Promise<UploadResult> {
		const files: File[] = [];
		for (let i = 0; i < fileList.length; i++) {
			files.push(fileList.item(i)!);
		}

		const formData = new FormData();
		for (const file of files) {
			formData.append('mediaFiles', file, file.name);
		}

		return this.http.post<UploadResult>(`${env.api.url}/media/upload`, formData).toPromise();
	}

	public deleteMedia(filename: string): Promise<object> {
		return this.http.delete(`${env.api.url}/media/${filename}`).toPromise();
	}
}
