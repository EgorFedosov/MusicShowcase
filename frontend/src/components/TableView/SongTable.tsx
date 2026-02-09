import { Table, Card, Tag } from "antd";
import { LikeFilled } from "@ant-design/icons";
import { SongDetails } from "./SongDetails";
import {
  DEFAULT_PAGE_SIZE,
  TOTAL_SONGS_COUNT,
  GENRE_COLORS,
} from "../../helpers/constants";
import type { ISong } from "../../types";
import type { ColumnsType } from "antd/es/table";
import s from "./SongTable.module.css";

const COLUMN_WIDTHS = {
  INDEX: 70,
  GENRE: 160,
  LIKES: 120,
};

const ALBUM_TYPE_SINGLE = "Single";

interface SongTableProps {
  songs: ISong[];
  loading: boolean;
  page: number;
  onPageChange: (page: number) => void;
}

export const SongTable = ({
  songs,
  loading,
  page,
  onPageChange,
}: SongTableProps) => {
  const columns: ColumnsType<ISong> = [
    {
      title: "#",
      dataIndex: "index",
      key: "index",
      width: COLUMN_WIDTHS.INDEX,
      align: "center",
      render: (text) => <span className={s.indexCol}>{text}</span>,
    },
    {
      title: "SONG",
      dataIndex: "title",
      key: "title",
      render: (text) => <span className={s.songTitle}>{text}</span>,
    },
    {
      title: "ARTIST",
      dataIndex: "artist",
      key: "artist",
      render: (text) => <span className={s.artist}>{text}</span>,
    },
    {
      title: "ALBUM",
      dataIndex: "album",
      key: "album",
      responsive: ["md"],
      render: (text) => (
        <span
          className={`${s.albumText} ${text === ALBUM_TYPE_SINGLE ? s.albumTextSingle : ""}`}
        >
          {text}
        </span>
      ),
    },
    {
      title: "GENRE",
      dataIndex: "genre",
      key: "genre",
      width: COLUMN_WIDTHS.GENRE,
      render: (genre) => (
        <Tag color={GENRE_COLORS[genre] || "default"} className={s.genreTag}>
          {genre}
        </Tag>
      ),
    },
    {
      title: "LIKES",
      dataIndex: "likes",
      key: "likes",
      align: "right",
      width: COLUMN_WIDTHS.LIKES,
      render: (likes) => (
        <span className={s.likes}>
          {likes} <LikeFilled className={s.likeIcon} />
        </span>
      ),
    },
  ];

  return (
    <Card
      variant="borderless"
      className={s.card}
      classNames={{ body: s.cardBody }}
    >
      <Table
        columns={columns}
        dataSource={songs}
        loading={loading}
        rowKey="index"
        size="middle"
        pagination={{
          current: page,
          pageSize: DEFAULT_PAGE_SIZE,
          total: TOTAL_SONGS_COUNT,
          onChange: onPageChange,
          showSizeChanger: false,
          position: ["bottomCenter"],
          className: s.pagination,
        }}
        expandable={{
          expandedRowRender: (record) => <SongDetails song={record} />,
          expandRowByClick: true,
          rowExpandable: () => true,
        }}
        rowClassName={() => "clickable-row"}
      />
    </Card>
  );
};
