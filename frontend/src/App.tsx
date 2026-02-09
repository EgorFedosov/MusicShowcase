import { useState } from "react";
import { ConfigProvider, Layout, FloatButton, message } from "antd";
import { UpOutlined } from "@ant-design/icons";
import { songsApi } from "./api/songsApi";
import { Toolbar } from "./components/Toolbar/Toolbar";
import { SongTable } from "./components/TableView/SongTable";
import { SongGallery } from "./components/GalleryView/SongGallery";
import { useSongs } from "./hooks/useSongs";
import {
  DEFAULT_LIKES,
  DEFAULT_REGION,
  DEFAULT_SEED,
} from "./helpers/constants";
import type { IGenerationParams } from "./types";
import "./App.css";

const { Content, Footer } = Layout;

function App() {
  const [view, setView] = useState<"table" | "gallery">("table");
  const [isExporting, setIsExporting] = useState(false);

  const [params, setParams] = useState<IGenerationParams>({
    seed: DEFAULT_SEED,
    region: DEFAULT_REGION,
    likesAverage: DEFAULT_LIKES,
    page: 1,
  });

  const { songs, loading, hasMore } = useSongs(params, view);

  const handleParamChange = (updates: Partial<IGenerationParams>) => {
    setParams((prev) => ({
      ...prev,
      ...updates,
      page: 1,
    }));
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  const handlePageChange = (page: number) => {
    setParams((prev) => ({ ...prev, page }));
    if (view === "table") {
      window.scrollTo({ top: 0, behavior: "smooth" });
    }
  };

  const handleGalleryLoadMore = () => {
    if (!loading) {
      setParams((prev) => ({ ...prev, page: prev.page + 1 }));
    }
  };

  const handleExport = async () => {
    try {
      setIsExporting(true);
      await songsApi.exportSongs(params);
      message.success("Export completed successfully");
    } catch (error) {
      console.error(error);
      message.error("Failed to export songs");
    } finally {
      setIsExporting(false);
    }
  };

  return (
    <ConfigProvider
      theme={{
        token: {
          colorPrimary: "#1890ff",
          borderRadius: 8,
          fontFamily:
            "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial",
        },
        components: {
          Layout: { bodyBg: "#f0f2f5", footerBg: "#f0f2f5" },
          Card: {
            boxShadowTertiary:
              "0 1px 2px 0 rgba(0, 0, 0, 0.03), 0 1px 6px -1px rgba(0, 0, 0, 0.02)",
          },
        },
      }}
    >
      <Layout className="app-layout">
        <Content className="app-content">
          <div className="toolbar-wrapper">
            <Toolbar
              params={params}
              view={view}
              onParamChange={handleParamChange}
              onViewChange={setView}
              onExport={handleExport}
              isExporting={isExporting}
            />
          </div>

          <div className="content-wrapper">
            {view === "table" ? (
              <SongTable
                songs={songs}
                loading={loading}
                page={params.page}
                onPageChange={handlePageChange}
              />
            ) : (
              <SongGallery
                songs={songs}
                hasMore={hasMore}
                loadMore={handleGalleryLoadMore}
              />
            )}
          </div>
        </Content>
        <Footer className="app-footer">
          Music Showcase Generator ©{new Date().getFullYear()}
        </Footer>
        <FloatButton.BackTop icon={<UpOutlined />} type="primary" />
      </Layout>
    </ConfigProvider>
  );
}

export default App;
