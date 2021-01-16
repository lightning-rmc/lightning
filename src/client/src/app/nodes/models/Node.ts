export type Node = {
	id: string;
	name: string;
	state: 'online' | 'offline' | 'error' | 'syncing';
}
