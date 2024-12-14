export interface Message {
  id: number;
  senderId: string;
  senderFirstName: string;
  senderPhotoUrl: string;
  recipientId: string;
  recipientFirstName: string;
  recipientPhotoUrl: string;
  content: string;
  dateRead: Date | null;
  messageSent: Date;
}
