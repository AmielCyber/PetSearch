import styles from "@/styles/layout/MainHeader.module.css";

type Props = {
  children: React.ReactNode;
};
export default function MainHeader(props: Props) {
  return <header className={styles.mainHeader}>{props.children}</header>;
}
