import Link from "next/link";
import styles from "@/styles/layout/NavigationLinks.module.css";

export default function NavigationLinks() {
  return (
    <div className={styles.headerNav}>
      <nav>
        <ul className={styles.pathList}>
          <li>
            <Link href="/">Home</Link>
          </li>
          <li>Location</li>
        </ul>
      </nav>
    </div>
  );
}
